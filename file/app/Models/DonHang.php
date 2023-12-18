<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\HasMany;

class DonHang extends Model
{
    protected $table = 'donhang';

    // Các trường có thể được gán
    protected $fillable = [
        'user_id',
        // Thêm các trường khác tương ứng với bảng donhang
    ];

    public function chiTietDonHang()
    {
        return $this->hasMany(ChiTietDonHang::class, 'donhang_id', 'id');
    }
    public function User(): BelongsTo
    {
        return $this->belongsTo(User::class, 'user_id', 'id');
    }
    public function TinhTrangDonHang(): BelongsTo
    {
        return $this->belongsTo(TinhTrangDonHang::class, 'tinhtrang_id', 'id');

    }
}
