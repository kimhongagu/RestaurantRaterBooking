<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Model;

class TacGia extends Model
{
    protected $table = 'tacgia'; // Tên bảng trong cơ sở dữ liệu

    protected $fillable = [
        'tentacgia',
        'tieusu',
        'hinhanh',
        
    ];

    public function sach()
    {
        return $this->hasMany(Sach::class, 'tacgia_id', 'id');
    }
}
